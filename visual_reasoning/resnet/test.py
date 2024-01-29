import torch
from data_loader import get_data_loader
from model import get_model

def test_model(model, data_loader):
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    model.to(device)

    model.eval()
    correct = 0
    total = 0

    with torch.no_grad():
        for images, labels in data_loader:
            images, labels = images.to(device), labels.to(device)
            outputs = model(images)
            _, predicted = torch.max(outputs.data, 1)
            total += labels.size(0)
            correct += (predicted == labels).sum().item()

    accuracy = correct / total
    print(f'Test Accuracy: {accuracy * 100:.2f}%')

if __name__ == "__main__":

    test_image_folder = "path/test/images"
    test_annotation_file = "path/test/annotations.txt"
    test_batch_size = 32

    # 获取测试数据加载器和模型
    test_data_loader = get_data_loader(test_image_folder, test_annotation_file, test_batch_size)
    trained_model = get_model()

    # 加载预训练权重或训练好的模型
    # trained_model.load_state_dict(torch.load("path/to/trained/model.pth"))

    test_model(trained_model, test_data_loader)
