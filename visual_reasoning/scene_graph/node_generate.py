import os
import csv
import torch
from PIL import Image
from torchvision import transforms
from openpose_model import detect_hand_keypoints
from utils import save_to_csv
import sys 
sys.path.append("..")
from resnet.model import CustomResNet50

transform = transforms.Compose([
    transforms.ToTensor()
])

def process_image(image_path):
    image = Image.open(image_path).convert("RGB")
    image_tensor = transform(image).unsqueeze(0)
    return image_tensor

def get_image_files(folder_path):
    if not os.path.exists(folder_path):
        print(f"Error: Folder '{folder_path}' not found.")
        return []

    files = os.listdir(folder_path)
    image_files = [file for file in files if file.lower().endswith(('.png', '.jpg', '.jpeg', '.gif', '.bmp'))]

    return image_files

def openpose_info_id(csv_filename, image_id):
    results = []
    with open(csv_filename, mode='r') as file:
        reader = csv.reader(file)
        header = next(reader)

        for row in reader:
            if row[-1] == image_id:

                x, y, w, h = map(float, row[:4])
                class_label = row[4]

                result = [x, y, w, h, class_label]
                results.append(result)

    return results

def graph_node(resnet_model, openpose_hand, image_path):

    resnet_model.eval()
    resnet_output = resnet_model(process_image(image_path))

    return integrate_results(resnet_output, openpose_hand)

def integrate_results(resnet_output, openpose_output):

    results = []
    boxes = resnet_output['boxes'].tolist()
    labels = resnet_output['labels'].tolist()
    for box, label in zip(boxes, labels):
        result = box + [label]
        results.append(result)
    results.append(openpose_output)


if __name__ == "__main__":
    
    resnet_model_path = "path/resnet_model.pth"
    openpose_info_path = "path/openpose_info.csv"
    image_folder = "path/images"
    output_csv_filename = "path/node_output"

    resnet_model = CustomResNet50()
    resnet_model.load_state_dict(torch.load(resnet_model_path))

    image_files = get_image_files(image_folder)

    for image_file in image_files:
        results = []
        hand_info = openpose_info_id(openpose_info_path, image_file)
        scene_graph_result = graph_node(resnet_model, hand_info, image_folder + '/' + image_file)
        results.extend(scene_graph_result)
        save_to_csv(results, image_file[:-3]+'csv')
