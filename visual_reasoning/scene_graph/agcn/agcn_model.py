import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F

class GCNLayer(nn.Module):
    def __init__(self, input_dim, output_dim):
        super(GCNLayer, self).__init__()
        self.linear = nn.Linear(input_dim, output_dim)

    def forward(self, adjacency_matrix, node_features):
        support = torch.mm(adjacency_matrix, node_features)
        output = self.linear(support)
        return output

class AttentionGCN(nn.Module):
    def __init__(self, input_dim, hidden_dim, output_dim):
        super(AttentionGCN, self).__init__()
        self.layer1 = GCNLayer(input_dim, hidden_dim)
        self.layer2 = GCNLayer(hidden_dim, hidden_dim)
        self.layer3 = GCNLayer(hidden_dim, output_dim)

    def forward(self, adjacency_matrix, node_features):
        h1 = F.relu(self.layer1(adjacency_matrix, node_features))
        h2 = F.relu(self.layer2(adjacency_matrix, h1))
        output = self.layer3(adjacency_matrix, h2)
        return output
