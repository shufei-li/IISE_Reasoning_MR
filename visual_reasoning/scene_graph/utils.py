import csv
import torch
import torch.nn as nn
import torch.optim as optim

def save_to_csv(results, csv_filename):
    with open(csv_filename, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerow(["x", "y", "w", "h", "class"])
        for result in results:
            writer.writerow(result)
