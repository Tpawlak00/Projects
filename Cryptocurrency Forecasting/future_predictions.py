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

data_test = df.loc['2021-01-01':'2022-01-01']['close']
data_test = np.array(data_test)

model = tf.keras.models.load_model('saved_model/MODEL1')
x_test, y_test = [], []
for i in range(pred_len, len(data_test)):
    x_test.append(data_test[i-pred_len:i])

print(pd.DataFrame(x_test))
x_test = np.array(x_test)
x_test = x_scaler.fit_transform(x_test)
x_test = x_test.reshape(len(x_test[:]), len(x_test[0]), 1)

# making test predictions to see how the models works
y_pred = model.predict(x_test)
y_pred = x_scaler.inverse_transform(y_pred)
test_pred = y_pred[::-1, 0]


# making future predictions by predicting a few times
future_pred = []
for i in range(0, pred_len):
    y_pred = x_scaler.transform(y_pred)
    y_pred = model.predict(y_pred)
    y_pred = x_scaler.inverse_transform(y_pred)
    for j in range(0, pred_len):
        future_pred.append(y_pred[-len(y_pred[::-1, 0])+pred_len-j, 0])

print(len(test_pred))
print(test_pred)
print(len(future_pred))
print(future_pred)

values = df['2021-01-01':'2022-03-22'][['close']].astype(float)

# making array of test values
add_dates = [values.index[-pred_len]+DateOffset(days=x) for x in range(0, len(test_pred)+1)]
dates = pd.DataFrame(index=add_dates[1:], columns=df.columns)
predict = pd.DataFrame(test_pred, index=dates.index, columns=['Test Prediction'])

# making array of future values
add_future_dates = [predict.index[-1]+DateOffset(days=x) for x in range(0, len(future_pred)+1)]
future_dates = pd.DataFrame(index=add_future_dates[1:], columns=df.columns)
df_predict = pd.DataFrame(future_pred, index=future_dates.index, columns=['Future Prediction'])

values.rename(columns={'close': 'Real Values'}, inplace=True)
values['Test Prediction'] = predict
values['Future Prediction'] = df_predict

values.rename(columns={'close': 'Real Values'}, inplace=True)
values.plot(figsize=(14, 5))
plt.show()
