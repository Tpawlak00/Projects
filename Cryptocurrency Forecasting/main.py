import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential
from tensorflow.keras.layers import LSTM, Dense

x_scaler = MinMaxScaler()
y_scaler = MinMaxScaler()
pred_length = 7

col_list = ['unix', 'date', 'symbol', 'open', 'high', 'low', 'close', 'Volume LTC', 'Volume USDT', 'tradecount']
df = pd.read_csv('./data/LTC_DATA/DailyData/Binance_LTCUSDT_d.csv', index_col='date', usecols=col_list,
                 low_memory=False, parse_dates=True)
df.drop(['symbol'], inplace=True, axis=1)

df = df.astype('float')

# df['close'].plot(figsize=(14, 5))
# plt.show()

data_train = df.loc['2017-12-31':'2020-12-31']['close']
data_test = df.loc['2021':]['close']
print(data_test.head())
print(data_train.head())

data_train = np.array(data_train)

x_train, y_train = [], []

for i in range(pred_length, len(data_train)-pred_length):
    x_train.append(data_train[i-pred_length:i])
    y_train.append(data_train[i:i+pred_length])

x_train, y_train = np.array(x_train), np.array(y_train)

# print(x_train.shape, y_train.shape)
print(pd.DataFrame(x_train))
print(pd.DataFrame(y_train))

x_train = x_scaler.fit_transform(x_train)
y_train = y_scaler.fit_transform(y_train)

x_train = x_train.reshape(len(x_train[:]), len(x_train[0]), 1)

model = Sequential()
model.add(LSTM(units=200, activation='relu', input_shape=(pred_length, 1)))
model.add(Dense(pred_length))
model.compile(loss='mse', optimizer='adam')

model.fit(y_train, x_train, epochs=30, batch_size=1)
model.save('saved_model/MODEL1')

data_test = np.array(data_test)

x_test, y_test = [], []
for i in range(7, len(data_test)):
    x_test.append(data_test[i-pred_length:i])
    y_test.append(data_test[i:i+pred_length])

print(pd.DataFrame(x_test))
print(pd.DataFrame(y_test))
x_test, y_test = np.array(x_test), np.array(x_test)
x_test = x_scaler.transform(x_test)
x_test = x_test.reshape(len(x_test[:]), len(x_test[0]), 1)

y_test = y_scaler.transform(y_test)
y_test = y_test.reshape(len(y_test[:]), len(y_test[0]), 1)

y_pred = model.predict(y_test)
y_pred = y_scaler.inverse_transform(y_pred)
print(y_pred)
pred = y_pred[:, pred_length-1]

values = df[0:len(pred)][['close']].astype(float)
values['Predicted'] = pred
print(values)
values.rename(columns={'close': 'Real Values'}, inplace=True)
values.plot(figsize=(14, 5))
plt.show()