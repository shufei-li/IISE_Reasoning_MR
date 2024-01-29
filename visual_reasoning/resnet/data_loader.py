import torch
from torch.utils.data import Dataset, DataLoader
from torchvision import transforms
from PIL import Image

class CustomDataset(Dataset):
    def __init__(self, image_folder, annotation_file, transform=None):

    def __len__(self):
        return len(self.data)

    def __getitem__(self, idx):

def get_data_loader(image_folder, annotation_file, batch_size=32):
    dataset = CustomDataset(image_folder, annotation_file, transform=transforms.Compose([transforms.ToTensor()]))
    data_loader = DataLoader(dataset, batch_size=batch_size, shuffle=True)
    return data_loader
