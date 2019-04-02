from __future__ import absolute_import, division, print_function

#!pip install tensorflow-gpu==2.0.0-alpha0
import tensorflow as tf
import numpy as np
import json

def normalise(number):
    return float(number)
    
def load_data():
    f = open("data/TrainData.txt", "r")
    x_train = []
    y_train = []
    for line in f:
        numbers = line.split(",")
        x_train.append([normalise(numbers[0]), normalise(numbers[1])])
        y_train.append(float(numbers[2]))
    f.close()
    x_test = []
    y_test = []
    f = open("data/TestData.txt", "r")
    for line in f:
        numbers = line.split(",")
        x_test.append([normalise(numbers[0]), normalise(numbers[1])])
        y_test.append(float(numbers[2]))
    return np.array(x_train), np.array(y_train), np.array(x_test), np.array(y_test)

mnist = tf.keras.datasets.mnist

x_train, y_train, x_test, y_test = load_data()
print("antall treningsdata: " + str(len(x_train)))
print("antall testdata: " + str(len(x_test)))

model = tf.keras.models.Sequential([
#   tf.keras.layers.Dense(4, activation='linear'),
#   tf.keras.layers.Dense(20, activation='linear'),
  tf.keras.layers.Dense(20, activation='linear'),
  tf.keras.layers.Dense(50, activation='linear'),
  tf.keras.layers.Dense(100, activation='linear'),
#   tf.keras.layers.Dense(100, activation='linear'),
  #tf.keras.layers.Dropout(0.9),
  tf.keras.layers.Dense(2, activation='softmax')
])

model.compile(optimizer='adam',
              loss='sparse_categorical_crossentropy',
              metrics=['accuracy'])

model.fit(x_train, y_train, epochs=100)

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

model.evaluate(x_test, y_test)

