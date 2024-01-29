import os
import numpy as np
import pandas as pd
from data_loader import load_data
from mlp_model import MLPModel
from mlp_train import train_model, get_similarity_matrix


def save_top_k_pairs(similarity_matrix, k, output_folder):
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    for i, similarity_matrix_i in enumerate(similarity_matrix):
        top_k_indices = np.argpartition(similarity_matrix_i, -k)[:, -k:]
        output_file = os.path.join(output_folder, f'result_{i}.csv')

        with open(output_file, 'w') as f:
            for idx in range(top_k_indices.shape[0]):
                for k_idx in top_k_indices[idx]:
                    f.write(f"{idx},{k_idx}\n")

# Set your data and label folders
data_folder = "path/to/data/folder"
label_folder = "path/to/label/folder"
output_folder = "path/to/output/folder"
k = 128

# Data loading
data_loader = load_data(data_folder, label_folder)

# Model definition
input_size = 5  # Number of features per node
hidden_size = 64
output_size = len(set(pd.read_csv(os.path.join(label_folder, os.listdir(label_folder)[0])).values.flatten()))  # Assuming unique labels

model = MLPModel(input_size, hidden_size, output_size)

# Training
train_model(data_loader, model)

# Get similarity matrix
similarity_matrix = get_similarity_matrix(data_loader, model)

# Save top k pairs
save_top_k_pairs(similarity_matrix, k, output_folder)