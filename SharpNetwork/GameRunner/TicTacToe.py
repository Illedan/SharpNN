from __future__ import absolute_import, division, print_function, unicode_literals
import tensorflow as tf
import numpy as np
import codecs, json

def normalise(number):
    if number == "2":
        return float(-1)
    return float(number)

def load_data():
    print("\nLeser inn test og treningsdata.")
    f = open("ttt_trainingData.txt", "r")
    x_train = []
    y_train = []
    for line in f:
        numbers = line.split(",")
        x_train.append([
            normalise(numbers[0]), 
            normalise(numbers[1]), 
            normalise(numbers[2]), 
            normalise(numbers[3]), 
            normalise(numbers[4]), 
            normalise(numbers[5]), 
            normalise(numbers[6]), 
            normalise(numbers[7]), 
            normalise(numbers[8]), 
            normalise(numbers[9]), 
            normalise(numbers[10]), 
            normalise(numbers[11]), 
            normalise(numbers[12]), 
            normalise(numbers[13]), 
            normalise(numbers[14]), 
            normalise(numbers[15]), 
            normalise(numbers[16]), 
            normalise(numbers[17]), 
            normalise(numbers[18]), 
            normalise(numbers[19]), 
            normalise(numbers[20]), 
            normalise(numbers[21]), 
            normalise(numbers[22]), 
            normalise(numbers[23]), 
            normalise(numbers[24]), 
            normalise(numbers[25]), 
            normalise(numbers[26])
            ])
        y_train.append(float(numbers[27]))
    f.close()
    x_test = []
    y_test = []
    f = open("ttt_testData.txt", "r")
    for line in f:
        numbers = line.split(",")
        x_test.append([
            normalise(numbers[0]), 
            normalise(numbers[1]), 
            normalise(numbers[2]), 
            normalise(numbers[3]), 
            normalise(numbers[4]), 
            normalise(numbers[5]), 
            normalise(numbers[6]), 
            normalise(numbers[7]), 
            normalise(numbers[8]), 
            normalise(numbers[9]), 
            normalise(numbers[10]), 
            normalise(numbers[11]), 
            normalise(numbers[12]), 
            normalise(numbers[13]), 
            normalise(numbers[14]), 
            normalise(numbers[15]), 
            normalise(numbers[16]), 
            normalise(numbers[17]), 
            normalise(numbers[18]), 
            normalise(numbers[19]), 
            normalise(numbers[20]), 
            normalise(numbers[21]), 
            normalise(numbers[22]), 
            normalise(numbers[23]), 
            normalise(numbers[24]), 
            normalise(numbers[25]), 
            normalise(numbers[26])
            ])
        y_test.append(float(numbers[27]))
    print("ferdig med å lese inn test og treningsdata.\n")
    return np.array(x_train), np.array(y_train), np.array(x_test), np.array(y_test)


x_train, y_train, x_test, y_test = load_data()
print("antall treningsdata: " + str(len(x_train)))
print("antall testdata: " + str(len(x_test)) + "\n")

model = tf.keras.models.Sequential([
 tf.keras.layers.Dense(100, activation='relu'),
 tf.keras.layers.Dropout(0.2),
 tf.keras.layers.Dense(9, activation='softmax')
])

model.compile(optimizer='adam',
             loss='sparse_categorical_crossentropy',
             metrics=['accuracy'])


y = model.fit(x_train, y_train, epochs=200)

#write_network_to_file(model)

model.evaluate(x_test, y_test)


weights_from_model = model.get_weights()
all_weights = []
bias_weights = []
yolo = []
for layer in weights_from_model:
   for weights in layer:
       if isinstance(weights, np.ndarray):
           temp = []
           for weight in weights:
               temp.append(str(weight))
               yolo.append(str(weight))
           all_weights.append(temp)
       else:
           bias_weights.append(str(weights))
           yolo.append(str(weights))

all_weights.append(bias_weights)
with open('weights.txt', 'w') as outfile:
   json.dump(yolo, outfile)

# serialize model to JSON
model_json = model.to_json()
with open("network.txt", "w") as json_file:
   json_file.write(model_json)
print("Done writing network to files.\n")