import numpy as np

a = np.array([[1,2,3], [9,7,8]])
b = np.array([[2,3,6], [2,3,6]])

print(np.append(a, b, axis=-1))