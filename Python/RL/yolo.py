from __future__ import absolute_import, division, print_function, unicode_literals
import tensorflow as tf
import numpy as np
import codecs, json
import operator
import random

model = tf.keras.models.Sequential([
tf.keras.layers.Dense(2, activation='relu'),
tf.keras.layers.Dense(2, activation='softmax')
])

model.compile(optimizer='adam',
            loss='sparse_categorical_crossentropy',
            metrics=['accuracy'])

observations = np.array([[1.0,2.0],[3.0,4.0]])
acts_and_advs = np.array([[0.0, 0.0], [1.0, 0.5]])
returns = np.array([0, 1])
print(observations)
print(acts_and_advs)
print(returns)
losses = model.train_on_batch(observations, returns, class_weight=acts_and_advs)
print("Losses: %s" % losses)