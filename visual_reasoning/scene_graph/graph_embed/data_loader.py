import pandas as pd
import torch
from torch.utils.data import Dataset, DataLoader

class GraphDataset(Dataset):
    def __init__(self, graph_folder, annotation_file):
        self.graph_folder = graph_folder
        self.annotation_file = annotation_file
        self.graph_files = [f"{graph_folder}/{i}.csv" for i in graph_folder]
        self.annotations = pd.read_csv(annotation_file)['label'].tolist()

    def __len__(self):
        return len(self.graph_files)

    def __getitem__(self, idx):
        graph_path = self.graph_files[idx]
        graph_data = pd.read_csv(graph_path, header=None)
        edges = graph_data.iloc[:2, :].values
        labels = graph_data.iloc[2, :].values
        labels = torch.tensor(labels, dtype=torch.long)
        return edges, labels

def load_data(graph_folder, annotation_file, batch_size=32):
    dataset = GraphDataset(graph_folder, annotation_file)
    dataloader = DataLoader(dataset, batch_size=batch_size, shuffle=True)
    return dataloader
