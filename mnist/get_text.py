import random
import os

number = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
path_dir = "data/"
if not os.path.exists(path_dir):
    os.makedirs(path_dir)


def random_number_text(char_set=number, code_size=1):
    code_text = []
    for each in range(code_size):
        c = random.choice(char_set)
        code_text.append(c)
    return code_text


def write_labels(size, name):
    code_list = []
    for each in range(size):
        number_list = random_number_text()
        code = ''.join(number_list) #将' '和number_list连成一个字符串
        code_list.append(code)#在code_list的结尾插入code
    code_text = ','.join(code_list)#code_text = '#'.join(code_list)
    print(code_text)
    f = open(path_dir + name, 'w')
    f.write(code_text)
    f.close()


def main():
    train_size = 5000
    test_size = 1000
    train_label_name = "code_train_text.txt"
    test_label_name = "code_test_text.txt"
    write_labels(train_size, train_label_name)
    write_labels(test_size, test_label_name)


if __name__ == '__main__':
    main()

