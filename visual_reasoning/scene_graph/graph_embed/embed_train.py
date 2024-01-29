import torch
import torch.optim as optim
import torch.nn as nn
from gcn import AttentionGCN
from data_loader import load_data

def train_model(graph_folder, annotation_file, input_size, hidden_size, output_size, num_epochs=10, lr=0.01, save_path='trained_model.pth'):
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')

    model = AttentionGCN(input_size, hidden_size, output_size).to(device)
    optimizer = optim.SGD(model.parameters(), lr=lr)
    criterion = nn.CrossEntropyLoss()

    dataloader = load_data(graph_folder, annotation_file)

    for epoch in range(num_epochs):
        for batch_edges, batch_labels in dataloader:
            adjacency_matrix = generate_adjacency_matrix(batch_edges)
            node_features = process_node_features(batch_edges)

            adjacency_matrix = adjacency_matrix.to(device)
            node_features = node_features.to(device)
            batch_labels = batch_labels.to(device)

            optimizer.zero_grad()
            outputs = model(adjacency_matrix, node_features)
            loss = criterion(outputs, batch_labels)
            loss.backward()
            optimizer.step()

        print(f'Epoch {epoch + 1}/{num_epochs}, Loss: {loss.item()}')

    # Save the trained model
    torch.save(model.state_dict(), save_path)
    print(f'Model saved to {save_path}')

def generate_adjacency_matrix(edges):
    num_nodes = edges.max().item() + 1
    adjacency_matrix = torch.zeros((num_nodes, num_nodes))

    for edge in edges.t():
        node1, node2 = edge[0].item(), edge[1].item()
        adjacency_matrix[node1, node2] = 1
        adjacency_matrix[node2, node1] = 1  # Assuming an undirected graph

    # Normalize the adjacency matrix
    row_sum = adjacency_matrix.sum(1, keepdim=True)
    adjacency_matrix = adjacency_matrix / row_sum

    return adjacency_matrix

def process_node_features(edges):
    num_nodes = edges.max().item() + 1
    # Assuming each node is represented by a one-hot encoded vector
    node_features = torch.eye(num_nodes)

    return node_features

if __name__ == "__main__":
    
    graph_folder='path/to/graphs'
    annotation_file='path/to/annotations.csv'
    input_size=32 
    hidden_size=64
    output_size=11
    num_epochs=20 
    lr=0.01

    train_model(graph_folder, annotation_file,
                input_size, hidden_size, output_size,
                num_epochs, lr)
