
import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F

def train(model, adjacency_matrix, node_features, ground_truth_edge_types, optimizer, criterion, epochs=100):
    model.train()

    for epoch in range(epochs):
        optimizer.zero_grad()
        output = model(adjacency_matrix, node_features)
        loss = criterion(output, ground_truth_edge_types)
        loss.backward()
        optimizer.step()

def test(model, adjacency_matrix, node_features):
    model.eval()
    output = model(adjacency_matrix, node_features)
    predicted_edge_types = torch.argmax(output, dim=1)
    return predicted_edge_types

def save_predicted_graph(file_path, node_pairs, predicted_edge_types):
    with open(file_path, 'w', newline='') as csv_file:
        csv_writer = csv.writer(csv_file)
        
        # Write potential connected node pairs with ID
        for node_pair in node_pairs:
            csv_writer.writerow(node_pair)

        # Write the predicted edge types
        csv_writer.writerow(predicted_edge_types.numpy())