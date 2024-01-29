import torch
import torch.optim as optim
import torch.nn as nn
from data_loader import get_data_loader
from model import get_model

def train_model(model, data_loader, num_epochs=10, learning_rate=0.001):
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    model.to(device)
    
    criterion = nn.CrossEntropyLoss()
    optimizer = optim.Adam(model.parameters(), lr=learning_rate)

    for epoch in range(num_epochs):
        for images, labels in data_loader:
            images, labels = images.to(device), labels.to(device)

            optimizer.zero_grad()
            outputs = model(images)
            loss = criterion(outputs, labels)
            loss.backward()
            optimizer.step()

        print(f'Epoch [{epoch+1}/{num_epochs}], Loss: {loss.item():.4f}')

if __name__ == "__main__":

    image_folder = "path/images"
    annotation_file = "path/annotations.txt"
    batch_size = 32
    num_epochs = 10

    # 获取数据加载器和模型
    data_loader = get_data_loader(image_folder, annotation_file, batch_size)
    model = get_model()

    train_model(model, data_loader, num_epochs)
