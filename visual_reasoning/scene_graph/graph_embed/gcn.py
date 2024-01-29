import torch
import torch.nn as nn
import torch.nn.functional as F

class GraphConvLayer(nn.Module):
    def __init__(self, in_features, out_features):
        super(GraphConvLayer, self).__init__()
        self.linear = nn.Linear(in_features, out_features)

    def forward(self, adjacency_matrix, node_features):
        adjacency_matrix = F.softmax(adjacency_matrix, dim=1)
        node_features = F.relu(self.linear(torch.matmul(adjacency_matrix, node_features)))
        return node_features

class AttentionGCN(nn.Module):
    def __init__(self, input_size, hidden_size, output_size):
        super(AttentionGCN, self).__init__()
        self.layer1 = GraphConvLayer(input_size, hidden_size)
        self.layer2 = GraphConvLayer(hidden_size, hidden_size)
        self.layer3 = GraphConvLayer(hidden_size, output_size)

    def forward(self, adjacency_matrix, node_features):
        h1 = self.layer1(adjacency_matrix, node_features)
        h2 = self.layer2(adjacency_matrix, h1)
        h3 = self.layer3(adjacency_matrix, h2)
        return h3
