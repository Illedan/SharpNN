from __future__ import absolute_import, division, print_function, unicode_literals
import tensorflow as tf
import numpy as np
import codecs, json
import operator
import random


class ProbabilityDistribution(tf.keras.Model):
  def call(self, logits, **kwargs):
    # Sample a random categorical action from the given logits.
    return tf.squeeze(tf.random.categorical(logits, 1), axis=-1)



class TicTacToe:
  possibleWins = [
              [0, 1, 2],
              [3, 4, 5],
              [6, 7, 8],
              [0, 3, 6],
              [1, 4, 7],
              [2, 5, 8],
              [0, 4, 8],
              [6, 4, 2]
          ]
  Board = None
 
  def getavailablemoves(self):
    return self.Board[18:len(self.Board)]
 
  def reset(self):
    self.Board = np.concatenate((np.zeros(18, dtype=float),np.ones(9,dtype=float)),axis=None)
    #return self.self.Board
 
  def step(self, a):
    offset=18
    self.Board[a] = 1.0
    if(a>8):
      offset=9
    self.Board[a+offset] = 0.0
 
    #return self.Board
 
  def isDone(self):
    if(self.isWinner(self.Board[0:9])):
      return True, 1
 
    if(self.isWinner(self.Board[9:18])):
      return True, 0
 
    if(np.any(self.Board[18:len(self.Board)] == 1.0)):
      return False, 0
    else:
      return True, 0.5
 
  def isWinner(self, PB):
    for win in self.possibleWins:
      if(PB[win[0]] == PB[win[1]] == PB[win[2]] == 1.0):
        return True
   
    return
    
  def player(self, i):
    if(self.Board[i+18]==1.0):
      return "_"
    if(self.Board[i+9]==1.0):
      return "X"
    if(self.Board[i]==1.0):
      return "O"
 
  def printBoard(self):
    print()
    for i in range(0,9,3):
      print("%s %s %s" % (self.player(i), self.player(i+1), self.player(i+2)))




def playOneGame(model, debug, isStarting):
    game = TicTacToe()
    game.reset()
    
    playerKJELL = isStarting
    starting = isStarting
    moves = []
    values = []
    actions = []
    while not game.isDone()[0]:
        if debug:
            game.printBoard()
        if playerKJELL:
            predictions = model.predict(np.array([game.Board]))
            availableMoves = game.getavailablemoves()
            for c in range(0, len(predictions[0])):
                if availableMoves[c] == 0.:
                    predictions[0][c] = 0.
            moveIndex, maxValue = max(enumerate(predictions[0]), key=operator.itemgetter(1))
            values.append(maxValue)
            game.step(moveIndex)
            moves.append(game.Board) #moveIndex
            actions.append(moveIndex)
        else:
            availableMoves = game.getavailablemoves()
            validMoves = []
            for c in range(0, len(availableMoves)):
                if availableMoves[c] == 1.:
                    validMoves.append(c)
            game.step(random.choice(validMoves)+9)
        playerKJELL = not playerKJELL
    score = game.isDone()[1]
    # print('Score=%s' % score)
    # print('Starting=%s' % starting)

    if starting:
        if score == 1:
            return 1, moves, game, values, actions
        elif score == 0.5:
            return 0.5, moves, game, values, actions
        else:
            return -1, moves, game, values, actions
    else:
        if score == 1:
            return -1, moves, game, values, actions
        elif score == 0.5:
            return 0.5, moves, game, values, actions
        else:
            return 1, moves, game, values, actions

    #return score, moves, game


def _returns_advantages(values, next_value):
    returns = np.append(np.zeros_like(values), [next_value], axis=-1)
    # print(returns)
    # print(values)
    
    for t in range(len(values)-1, -1, -1):
        returns[t] = 0.99 * returns[t + 1]
        
    returns = returns[:-1]
    advantages = returns - values
    return np.array(returns), np.array(advantages)

if __name__ == "__main__":
    model = tf.keras.models.Sequential([
    tf.keras.layers.Dense(100, activation='relu'),
    tf.keras.layers.Dense(9, activation='softmax')
    ])
    
    model.compile(optimizer='adam',
                loss='sparse_categorical_crossentropy',
                metrics=['accuracy'])
    #logits = kl.Dense(9, name='policy_logits')
    #dist = ProbabilityDistribution()
    batchSize = 64

    for i in range(10):
        print(i)
        for step in range(0, int(batchSize)):
            observations = []
            actRes = []
            val = random.getrandbits(1)
            score, moves, game, values, actions = playOneGame(model, False, bool(val))
            #if score < 0.6:
            #    continue

            for move in moves:
                observations.append(np.array(move))
            for act in actions:
                actRes.append(act)
                #print(np.array(observations))
                #print(actions)
    #model.fit(np.array(observations), np.array(actRes), epochs=50)
#fit(x=None, y=None, batch_size=None, epochs=1, verbose=1, callbacks=None, validation_split=0.0, validation_data=None, shuffle=True, class_weight=None, sample_weight=None, initial_epoch=0, steps_per_epoch=None, validation_steps=None, validation_freq=1, max_queue_size=10, workers=1, use_multiprocessing=False)                
            returns, advs = _returns_advantages(values, score)
            acts_and_advs = np.concatenate([np.array(actions)[:, None], advs[:, None]], axis=-1)
            # print('obs')
            # print(np.array(observations))
            # # print('acts_and_advs')
            # print(np.array(acts_and_advs))
            # # print('returns')
            # print(np.array(returns))
            # print('combined')
            # print([acts_and_advs, returns])
            losses = model.train_on_batch(np.array(observations), np.array(returns), class_weight=acts_and_advs)
            # print("Losses: %s" % losses)
        


        #for index in range(len(moves)):
        #    model.fit(moves[index][0], moves[index][0])
       # print('moves')
       # print(moves[0][0])


        # _, next_value = model.action_value(next_obs[None, :])
        # returns, advs = self._returns_advantages(rewards, dones, values, next_value)
        # # A trick to input actions and advantages through same API.
        # acts_and_advs = np.concatenate([actions[:, None], advs[:, None]], axis=-1)
        # # Performs a full training step on the collected batch.
        # # Note: no need to mess around with gradients, Keras API handles it.
        # losses = self.model.train_on_batch(observations, [acts_and_advs, returns])
        # logging.debug("[%d/%d] Losses: %s" % (update + 1, updates, losses))
        
    #model.fit(board,result)
    #print(board,result)
    wins = 0
    loss = 0
    draw = 0
    isStarting = 0
    for _ in range(100):
        val = random.getrandbits(1)
        score, moves, game, values, actions = playOneGame(model, False, bool(val))
        
        if bool(val):
            isStarting += 1
        if score == 1:
            wins += 1
        elif score == 0.5:
            draw += 1
        else:
            loss += 1
           
    #score, moves, game, values, actions = playOneGame(model, True, True) 
    # if score == 0.5:
    #     print('Draw')
    # elif score == 1:
    #     print('Player 1 won')
    # else:
    #     print('Player 1 lost')
    # print('Score=%s' % score)
    # print(moves)
    print('WINS: %s' % wins)
    print('LOSS: %s' % loss)
    print('DRAW: %s' % draw)
    print('Starting: %s' % isStarting)
    
    game.printBoard()