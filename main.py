import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'
import tensorflow as tf
from tensorflow.keras.preprocessing.text import Tokenizer
import numpy as np
import pandas as pd
from collections import Counter
from tensorflow.keras.preprocessing.sequence import pad_sequences
from tensorflow.keras import layers

def counter_word(text_col):
    count = Counter()
    for text in text_col.values:
        for word in text.split():
            count[word] += 1
    return count

df = pd.read_csv('./data/dataset.csv', usecols=["question", "target"], encoding="utf-8")

counter = counter_word(df.question)
print(counter.most_common(5))
print(len(counter))
num_unique_words = len(counter)

print(df.head())
print((df.target == 1).sum()) # Correct Questions
print((df.target == 0).sum()) # Incorrect Questions

train_size = int(df.shape[0] * 0.8)
train_df = df[:train_size]
val_df = df[train_size:]

train_questions = train_df.question.to_numpy()
train_targets = train_df.target.to_numpy()
val_questions = val_df.question.to_numpy()
val_targets = val_df.target.to_numpy()

tokenizer = Tokenizer(num_words=num_unique_words)
tokenizer.fit_on_texts(train_questions)
word_index = tokenizer.word_index

train_sequences = tokenizer.texts_to_sequences(train_questions)
val_sequences = tokenizer.texts_to_sequences(val_questions)

max_length = 20

train_padded = pad_sequences(train_sequences, maxlen=max_length, padding="post", truncating="post")
val_padded = pad_sequences(val_sequences, maxlen=max_length, padding="post", truncating="post")

reverse_word_index = dict([(idx, word) for (word, idx) in word_index.items()])
def decode(sequence):
    return " ".join([reverse_word_index.get(idx, "?") for idx in sequence])

decoded_text = decode(train_sequences[10])

model = tf.keras.models.Sequential()
model.add(layers.Embedding(num_unique_words, 32, input_length=max_length))
model.add(layers.LSTM(64, dropout=0.1))
model.add(layers.Dense(1, activation="sigmoid"))

model.summary()
loss = tf.keras.losses.BinaryCrossentropy(from_logits=False)
optim = tf.keras.optimizers.Adam(lr=0.001)
metrics = ["accuracy"]

model.compile(loss=loss, optimizer=optim, metrics=metrics)

model.fit(train_padded, train_targets, epochs=20, validation_data=(val_padded, val_targets), verbose=2)
predictions = model.predict(train_padded)
predictions = [1 if p > 0.5 else 0 for p in predictions]

print(train_questions[10:20])
print(train_targets[10:20])
print(predictions[10:20])