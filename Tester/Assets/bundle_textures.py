from PIL import Image
import os
import json
import shutil

class AtlasSheetMeta:
    def __init__(self):
        self.frames = []

    class FrameData:
        def __init__(self, filename, x, y, width, height):
            self.filename = filename
            self.frame = self.Rectangle(x, y, width, height)

        class Rectangle:
            def __init__(self, x, y, width, height):
                self.x = x
                self.y = y
                self.width = width
                self.height = height

def create_spritesheets(input_dir, output_dir):
    images = []
    max_width, max_height = 1024 * 2, 1024 * 2
    spritesheet_index = 1

    for filename in sorted(os.listdir(input_dir), key=lambda f: os.path.getsize(os.path.join(input_dir, f))):
        if filename.lower().endswith(('.png', '.jpg', '.jpeg')):
            image_path = os.path.join(input_dir, filename)
            img = Image.open(image_path)
            images.append(img)

    def save_spritesheet(spritesheet, metadata):
        nonlocal spritesheet_index
        output_spritesheet_path = os.path.join(output_dir, f'spritesheet_{spritesheet_index}.png')
        spritesheet.save(output_spritesheet_path)

        output_json_path = os.path.join(output_dir, f'spritesheet_{spritesheet_index}.json')
        with open(output_json_path, 'w') as f:
            json.dump(metadata.__dict__, f, default=lambda o: o.__dict__, indent=2)

        spritesheet_index += 1

    spritesheet = Image.new('RGBA', (max_width, max_height), (0, 0, 0, 0))
    free_rectangles = [(0, 0, max_width, max_height)]
    metadata = AtlasSheetMeta()

    for img in images:
        best_fit = None

        for rect in free_rectangles:
            if img.width <= rect[2] and img.height <= rect[3]:
                if best_fit is None or rect[2] * rect[3] < best_fit[2] * best_fit[3]:
                    best_fit = rect

        if best_fit is None:
            # Save current spritesheet and metadata, start a new spritesheet
            save_spritesheet(spritesheet, metadata)

            spritesheet = Image.new('RGBA', (max_width, max_height), (0, 0, 0, 0))
            free_rectangles = [(0, 0, max_width, max_height)]
            metadata = AtlasSheetMeta()

            # Try to fit the image again in the new spritesheet
            for rect in free_rectangles:
                if img.width <= rect[2] and img.height <= rect[3]:
                    if best_fit is None or rect[2] * rect[3] < best_fit[2] * best_fit[3]:
                        best_fit = rect

        if best_fit is not None:
            x, y, _, _ = best_fit

            spritesheet.paste(img, (x, y))
            frame_data = AtlasSheetMeta.FrameData(img.filename, x, y, img.width, img.height)
            metadata.frames.append(frame_data)

            new_rectangles = []
            for rect in free_rectangles:
                if rect != best_fit:
                    new_rectangles.append(rect)
                else:
                    if rect[2] > img.width:
                        new_rectangles.append((x + img.width, y, rect[2] - img.width, img.height))
                    if rect[3] > img.height:
                        new_rectangles.append((x, y + img.height, rect[2], rect[3] - img.height))

            free_rectangles = new_rectangles

    # Save the last spritesheet and metadata
    save_spritesheet(spritesheet, metadata)

# Example usage:
input_directory = './Textures/'
output_directory = './Textures/output'

shutil.rmtree(output_directory)
os.makedirs(output_directory)

create_spritesheets(input_directory, output_directory)
