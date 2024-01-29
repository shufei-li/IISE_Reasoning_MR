import pandas as pd

def evaluate_predictions(predicted_file_path, ground_truth_file_path):
    # Read predicted and ground truth CSV files into pandas DataFrames
    predicted_df = pd.read_csv(predicted_file_path, header=None)
    ground_truth_df = pd.read_csv(ground_truth_file_path, header=None)

    # Extracting node pairs and edge types from DataFrames
    predicted_node_pairs = predicted_df.iloc[:2, :].transpose()
    predicted_edge_types = predicted_df.iloc[2, :]

    ground_truth_node_pairs = ground_truth_df.iloc[:2, :].transpose()
    ground_truth_edge_types = ground_truth_df.iloc[2, :]

    # Calculate the number of correct linked edges
    correct_linked_edges = len(set(map(tuple, predicted_node_pairs.values)) & set(map(tuple, ground_truth_node_pairs.values)))

    # Calculate the number of correctly predicted edge types
    correct_edge_types = (predicted_edge_types == ground_truth_edge_types).sum()

    # Print the results
    print("Number of Correct Linked Edges:", correct_linked_edges)
    print("Number of Correctly Predicted Edge Types:", correct_edge_types)

# Example usage
predicted_file_path = 'path/predicted.csv'
ground_truth_file_path = 'path/ground_truth.csv'
evaluate_predictions(predicted_file_path, ground_truth_file_path)
