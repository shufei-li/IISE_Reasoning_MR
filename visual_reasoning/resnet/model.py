import torch
import torch.nn as nn
from torchvision.models import resnet50

class CustomResNet50(nn.Module):
    def __init__(self, num_classes=13):
        super(CustomResNet50, self).__init__()
        # 使用预训练的 ResNet-50，并替换最后一层分类器
        resnet = resnet50(pretrained=True)
        self.features = nn.Sequential(*list(resnet.children())[:-1])
        self.classifier = nn.Linear(resnet.fc.in_features, num_classes)

    def forward(self, x):
        x = self.features(x)
        x = x.view(x.size(0), -1)
        x = self.classifier(x)
        return x

def get_model():
    model = CustomResNet50()
    return model
