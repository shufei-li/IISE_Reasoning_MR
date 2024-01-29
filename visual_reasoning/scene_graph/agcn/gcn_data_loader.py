import os
import json
import numpy as np
import pandas as pd

def load_data(graph_folder, node_folder, annotation_file):
    graph_data = []
    node_features = {}

    # Load node features from JSON files
    for node_file in os.listdir(node_folder):
        if node_file.endswith(".json"):
            with open(os.path.join(node_folder, node_file), "r") as json_file:
                node_id = int(os.path.splitext(node_file)[0])
                features = json.load(json_file)
                node_features[node_id] = features

    # Load graph data from CSV files
    for graph_file in os.listdir(graph_folder):
        if graph_file.endswith(".csv"):
            graph_path = os.path.join(graph_folder, graph_file)
            with open(graph_path, "r") as csv_file:
                # Read potential connected node pairs and edge types
                graph_reader = pd.read_csv(csv_file, header=None)
                node_pairs = graph_reader.iloc[:2, :].values.transpose()
                edge_types = graph_reader.iloc[2, :].values

                # Load ground truth from annotation CSV file
                annotation_path = os.path.join(annotation_file, graph_file)
                with open(annotation_path, "r") as annotation_csv:
                    annotation_reader = pd.read_csv(annotation_csv, header=None)
                    ground_truth_node_pairs = annotation_reader.iloc[:2, :].values.transpose()
                    ground_truth_edge_types = annotation_reader.iloc[2, :].values

                    # Create adjacency matrix
                    num_nodes = max(np.max(ground_truth_node_pairs))
                    adjacency_matrix = create_adjacency_matrix(
                        ground_truth_node_pairs, ground_truth_edge_types, num_nodes
                    )

                    graph_data.append({
                        'node_pairs': node_pairs,
                        'edge_types': edge_types,
                        'ground_truth_node_pairs': ground_truth_node_pairs,
                        'ground_truth_edge_types': ground_truth_edge_types,
                        'adjacency_matrix': adjacency_matrix
                    })

    return graph_data, node_features, annotation_file

