from __future__ import absolute_import, division, print_function
import tensorflow as tf
import numpy as np
import codecs, json 
def normalise(number):
    return (float(number) + 10000)/100000

def load_data():
    print("\nLeser inn test og treningsdata.")
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
    print("ferdig med å lese inn test og treningsdata.\n")
    return np.array(x_train), np.array(y_train), np.array(x_test), np.array(y_test)

def write_network_to_file(model):
    print("\nwriting network to files.")
    number_of_layers = len(model.layers)

    all_weights = []
    for i in range(0, len(model.layers)):
        layer = model.get_layer(index=i)
        all_weights_per_layer = []
        bias_weights_per_layer = []
        for weights_per_layer in layer.get_weights():       
            for weights in weights_per_layer:
                if isinstance(weights, np.ndarray):
                    temp = []
                    for weight in weights:
                        temp.append(str(weight))            
                    if len(temp) > 0:
                        all_weights_per_layer.append(temp)
                else:
                    bias_weights_per_layer.append(str(weights))

        all_weights_per_layer.append(bias_weights_per_layer)
        all_weights.append(all_weights_per_layer)

    with open('weights.txt', 'w') as outfile:  
        json.dump(all_weights, outfile)
    
    # serialize model to JSON
    model_json = model.to_json()
    with open("model.json", "w") as json_file:
        json_file.write(model_json)
    print("Done writing network to files.\n")

x_train, y_train, x_test, y_test = load_data()
print("antall treningsdata: " + str(len(x_train)))
print("antall testdata: " + str(len(x_test)) + "\n")


model = tf.keras.models.Sequential([
  tf.keras.layers.Dense(100, activation='linear'),
  tf.keras.layers.Dense(100, activation='linear'),
  #tf.keras.layers.Dropout(0.2),
  tf.keras.layers.Dense(2, activation='softmax')
])

model.compile(optimizer='adam',
              loss='sparse_categorical_crossentropy',
              metrics=['accuracy'])


y = model.fit(x_train, y_train, epochs=20)

write_network_to_file(model)

model.evaluate(x_test, y_test)