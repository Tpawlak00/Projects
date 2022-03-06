import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
from pandas.tseries.offsets import DateOffset
import pandas as pd
import matplotlib.pyplot as plt
import tensorflow as tf
import numpy as np
from sklearn.preprocessing import MinMaxScaler

scaler = MinMaxScaler()
pred_len = 7
col_list = ['unix', 'date', 'symbol', 'open', 'high', 'low', 'close', 'Volume LTC', 'Volume USDT', 'tradecount']
df = pd.read_csv('./data/LTC_DATA/DailyData/Binance_LTCUSDT_d2.csv', index_col='date', usecols=col_list,
                 low_memory=False, parse_dates=True)
df.drop(['symbol'], inplace=True, axis=1)
df = df.astype('float')

values = df[0:3*pred_len][['close']].astype(float)
values.rename(columns={'close': 'Real Values'}, inplace=True)
data_test = df.loc['2021':]['close']
data_test = np.array(data_test)

x_test, y_test = [], []
for i in range(pred_len, len(data_test)):
    x_test.append(data_test[i-pred_len:i])
    y_test.append(data_test[i:i+pred_len])

print(pd.DataFrame(x_test))
print(pd.DataFrame(y_test))
x_test = np.array(x_test)
print(x_test[:][0])
x_test = scaler.fit_transform(x_test)
batch = x_test[:][0:2*pred_len]
print(batch)
batch = np.array(batch)
batch = batch.reshape(len(x_test[0:2*pred_len]), len(x_test[0]), 1)

pred_list = []

model = tf.keras.models.load_model('saved_model/MODEL1')
pred = model.predict(batch)
print(pred)
pred = scaler.inverse_transform(pred)
print(pred)

for i in range(0, 2*pred_len):
    pred_list.append(pred[i][0])

print(pred_list)
add_dates = [df.index[0]+DateOffset(days=x) for x in range(0, 2*pred_len+1)]
future_dates = pd.DataFrame(index=add_dates[1:], columns=df.columns)
df_predict = pd.DataFrame(pred_list,
                          index=future_dates[:2*pred_len].index,
                          columns=['Prediction'])
df_plot = pd.concat([values, df_predict], axis=1)
print(df_plot)
df_plot.plot(figsize=(14, 5))
plt.show()
