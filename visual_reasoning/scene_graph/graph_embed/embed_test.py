import torch
import torch.nn as nn
from gcn import AttentionGCN
from data_loader import load_data
from embed_train import generate_adjacency_matrix, process_node_features

def test_model(model, test_dataloader):
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
    model.to(device)
    model.eval()  # Set the model to evaluation mode

    correct_predictions = 0
    total_samples = 0

    with torch.no_grad():
        for batch_edges, batch_labels in test_dataloader:
            adjacency_matrix = generate_adjacency_matrix(batch_edges)
            node_features = process_node_features(batch_edges)

            adjacency_matrix = adjacency_matrix.to(device)
            node_features = node_features.to(device)
            batch_labels = batch_labels.to(device)

            outputs = model(adjacency_matrix, node_features)
            _, predicted_labels = torch.max(outputs, 1)

            correct_predictions += (predicted_labels == batch_labels).sum().item()
            total_samples += batch_labels.size(0)

    accuracy = correct_predictions / total_samples
    print(f'Test Accuracy: {accuracy * 100:.2f}%')

if __name__ == "__main__":

    input_size=32 
    hidden_size=64
    output_size=11

    # Load the trained model
    model = AttentionGCN(input_size, hidden_size, output_size)
    model.load_state_dict(torch.load('path/trained_model.pth'))

    # Set up the test data loader
    test_dataloader = load_data(graph_folder='path/test_graphs', annotation_file='path/test_annotations.csv')

    # Test the model
    test_model(model, test_dataloader)
