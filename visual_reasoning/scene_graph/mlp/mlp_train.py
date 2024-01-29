import torch
import torch.optim as optim
import torch.nn.functional as F
from mlp_model import MLPModel
from data_loader import load_data

def train_model(data_loader, model, lr=0.001, epochs=10):
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    model.to(device)
    optimizer = optim.SGD(model.parameters(), lr=lr)
    criterion = nn.CrossEntropyLoss()

    for epoch in range(epochs):
        for inputs, labels in data_loader:
            inputs, labels = inputs.float().to(device), labels.to(device)
            optimizer.zero_grad()
            outputs = model(inputs)
            loss = criterion(outputs, labels)
            loss.backward()
            optimizer.step()

def get_similarity_matrix(data_loader, model):
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    model.to(device)
    model.eval()
    similarity_matrix = []

    with torch.no_grad():
        for inputs, _ in data_loader:
            inputs = inputs.float().to(device)
            outputs = model(inputs)
            similarity_matrix.append(F.softmax(outputs, dim=1).cpu().numpy())

    return similarity_matrix
