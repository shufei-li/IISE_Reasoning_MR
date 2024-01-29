import os
import pandas as pd
import torch
from torch.utils.data import Dataset, DataLoader

class NodeDataset(Dataset):
    def __init__(self, data_folder, label_folder):
        self.data_folder = data_folder
        self.label_folder = label_folder
        self.file_list = os.listdir(data_folder)

    def __len__(self):
        return len(self.file_list)

    def __getitem__(self, idx):
        data_path = os.path.join(self.data_folder, self.file_list[idx])
        label_path = os.path.join(self.label_folder, self.file_list[idx])

        data = pd.read_csv(data_path)
        labels = pd.read_csv(label_path)

        return torch.tensor(data.values), torch.tensor(labels.values.flatten())

def load_data(data_folder, label_folder, batch_size=32, shuffle=True):
    dataset = NodeDataset(data_folder, label_folder)
    data_loader = DataLoader(dataset, batch_size=batch_size, shuffle=shuffle)
    return data_loader
