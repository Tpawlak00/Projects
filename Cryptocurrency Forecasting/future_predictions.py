from pandas.tseries.offsets import DateOffset
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.preprocessing import MinMaxScaler
from tensorflow.keras import Sequential

scaler = MinMaxScaler()
pred_len = 7
col_list = ['unix', 'date', 'symbol', 'open', 'high', 'low', 'close', 'Volume LTC', 'Volume USDT', 'tradecount']
df = pd.read_csv('./data/LTC_DATA/DailyData/Binance_LTCUSDT_d.csv', index_col='date', usecols=col_list,
                 low_memory=False, parse_dates=True)
df.drop(['symbol'], inplace=True, axis=1)
df = df.astype('float')

values = df[0:pred_len][['close']].astype(float)

train = df[['close']]
print(train)
scaler.fit(train)
train = scaler.transform(train)

x_train, y_train = [], []
for i in range(pred_len, len(train)-pred_len):
    x_train.append(train[i-pred_len:i])
    y_train.append(train[i:i+pred_len])


pred_list = []
model = Sequential()
model.load('saved_model/MODEL1')
for i in range(0, pred_len):
    pred_list.append(model.predict(x_train[i,0]))

add_dates = [df.index[-1]+DateOffset(days=x) for x in range(0, pred_len)]
future_dates = pd.DataFrame(index=add_dates[1:], columns=df.columns)
df_predict = pd.DataFrame(scaler.inverse_transform(pred_list),
                          index=future_dates[-1].index,
                          columns=['Prediction'])
df_plot = pd.concat([values, df_predict], axis=1)

values.plot(figsize=(14, 5))
plt.show()
