import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
from pandas.tseries.offsets import DateOffset
import pandas as pd
import matplotlib.pyplot as plt
import tensorflow as tf
import numpy as np
from sklearn.preprocessing import MinMaxScaler

x_scaler = MinMaxScaler()
y_scaler = MinMaxScaler()

pred_len = 7
col_list = ['unix', 'date', 'symbol', 'open', 'high', 'low', 'close', 'Volume LTC', 'Volume USDT', 'tradecount']
df = pd.read_csv('./data/LTC_DATA/DailyData/Binance_LTCUSDT_d3.csv', index_col='date', usecols=col_list,
                 low_memory=False, parse_dates=True)
df.drop(['symbol'], inplace=True, axis=1)
df = df.astype('float')

values = df[0:6*pred_len][['close']].astype(float)
values.rename(columns={'close': 'Real Values'}, inplace=True)
data_test = df.loc['2021':]['close']
data_test = np.array(data_test)

model = tf.keras.models.load_model('saved_model/MODEL1')
x_test, y_test = [], []
for i in range(pred_len, len(data_test)):
    x_test.append(data_test[i-pred_len:i])
    y_test.append(data_test[i:i + pred_len])

print(pd.DataFrame(x_test))
print(pd.DataFrame(y_test))
x_test = np.array(x_test)
x_test = x_scaler.fit_transform(x_test)
x_test = x_test.reshape(len(x_test[:]), len(x_test[0]), 1)

y_pred = model.predict(x_test)
y_pred = x_scaler.inverse_transform(y_pred)
pred = y_pred[:, pred_len-1]
print(y_pred)
add_dates = [df.index[0]+DateOffset(days=x) for x in range(0, pred_len+1)]
future_dates = pd.DataFrame(index=add_dates[1:], columns=df.columns)
df_predict = pd.DataFrame(y_pred[0:pred_len,0],
                          index=future_dates[:pred_len].index,
                          columns=['Future Prediction'])
values = df[0:len(pred)][['close']].astype(float)
values['Test Prediction'] = pred
df_plot = pd.concat([values, df_predict], axis=1)
print(df_plot.tail(20))
df_plot.rename(columns={'close': 'Real Values'}, inplace=True)
df_plot.plot(figsize=(14, 5))
plt.show()
