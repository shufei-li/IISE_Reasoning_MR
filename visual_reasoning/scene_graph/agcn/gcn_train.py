from gcn_data_loader import load_data
from agcn import AttentionGCN
from utils import train, test, save_predicted_graph
import torch
import torch.nn as nn
import torch.optim as optim


def create_adjacency_matrices(graph_data, node_features):
    adjacency_matrices = []

    for graph in graph_data:
        # Extracting ground truth node pairs and edge types
        ground_truth_node_pairs = graph['ground_truth_node_pairs']
        ground_truth_edge_types = graph['ground_truth_edge_types']

        # Creating a graph and adding edges with edge types
        G = nx.Graph()
        for i, (node_pair, edge_type) in enumerate(zip(ground_truth_node_pairs, ground_truth_edge_types)):
            node1, node2 = node_pair
            G.add_edge(node1, node2, edge_type=edge_type)

        # Creating adjacency matrix
        adjacency_matrix = nx.adjacency_matrix(G, nodelist=list(node_features.keys()))
        adjacency_matrices.append({
            'adjacency_matrix': adjacency_matrix,
            'ground_truth_edge_types': ground_truth_edge_types
        })

    return adjacency_matrices


graph_folder = "path/train/graphs"
node_folder = "path/train/node_f"
graph_test_folder = "path/test/graphs"
node_test_folder = "path/test/node_f"
annotation_file = "path/to/annotation.csv"

# Initialize and train the GCN model
input_dim = 5  # 5 features for each node
hidden_dim = 16
output_dim = 7  # Number of edge types
model = AttentionGCN(input_dim, hidden_dim, output_dim)
optimizer = optim.SGD(model.parameters(), lr=0.001)
criterion = nn.CrossEntropyLoss()

graph_data, node_features, annotation_file = load_data(graph_folder, node_folder, annotation_file)

for graph in graph_data:
    adjacency_matrices = create_adjacency_matrices(graph_data, node_features)
    ground_truth_edge_types = torch.tensor(graph['ground_truth_edge_types'])
    node_features_tensor = torch.tensor([node_features[node] for node in adjacency_matrices.indices()], dtype=torch.float32)
    
    train(model, adjacency_matrices, node_features_tensor, ground_truth_edge_types, optimizer, criterion)

test_data, test_features, annotation_file = load_data(graph_folder, node_folder, annotation_file)
# Test the model
for graph in test_data:
    adjacency_matrices = create_adjacency_matrices(graph_data, node_features)
    node_features_tensor = torch.tensor([node_features[node] for node in adjacency_matrices.indices()], dtype=torch.float32)
    
    predicted_edge_types = test(model, adjacency_matrices, node_features_tensor)

    # Calculate the number of correct linked edges
    correct_edges = torch.sum(predicted_edge_types == ground_truth_edge_types).item()

    # Save the predicted graph to CSV
    save_predicted_graph(f"predicted_graph_{graph_data.index(graph)}.csv", graph['node_pairs'], predicted_edge_types)

