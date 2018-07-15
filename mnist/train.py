import tensorflow as tf
import numpy as np
from PIL import Image
import random
from tensorflow.python.framework import graph_util

IMAGE_MUMBER = 59999
EPOCH = 30
BATCH_SIZE = 100
IMAGE_PATH = "F:/mnist/data/image/train/"
LABEL_PATH = "F:/mnist/data/code_train_text.txt"

def weigth_variable(shape):
    initial = tf.truncated_normal(shape, stddev=0.1) 
    return tf.Variable(initial)

def bias_varibale(shape):
    initial = tf.constant(0.1, shape=shape)
    return tf.Variable(initial)

def conv2d(x, W):
    return tf.nn.conv2d(x, W, strides=[1, 1, 1, 1], padding='SAME')

def max_pool_2x2(x):
    return tf.nn.max_pool(x, ksize=[1, 2, 2, 1], strides=[1, 2, 2, 1], padding='SAME')


IMAGE_HEIGHT = 28
IMAGE_WIDTH = 28
CHAR_SET_LEN = 10
xs = tf.placeholder(tf.float32, [None, IMAGE_HEIGHT * IMAGE_WIDTH],name='input')#input在这吗 声明一个float32类型的未知宽度，长度为28*28的矩阵形状，名字叫input
ys = tf.placeholder(tf.float32, [None, 10],name='labels')
keep_prob = tf.placeholder(tf.float32, name='keep_prob')
x_image = tf.reshape(xs, [-1, IMAGE_HEIGHT, IMAGE_WIDTH, 1])#将input矩阵的形状变成一个四维张量


def code_cnn():
    # 1 
    W_conv1 = weigth_variable([5, 5, 1, 32])
    b_conv1 = weigth_variable([32])
    h_conv1 = tf.nn.relu(conv2d(x_image, W_conv1) + b_conv1)  
    h_pool1 = max_pool_2x2(h_conv1) 
    # 2
    W_conv2 = weigth_variable([5, 5, 32, 64])
    b_conv2 = weigth_variable([64])
    h_conv2 = tf.nn.relu(conv2d(h_pool1, W_conv2) + b_conv2)  
    h_pool2 = max_pool_2x2(h_conv2)  
    h_pool2 = tf.nn.dropout(h_pool2, keep_prob)
    # 3
    W_fc1 = weigth_variable([7 * 7 * 64, 1024])
    b_fc1 = bias_varibale([1024])
    h_pool2_flat = tf.reshape(h_pool2, [-1, 7 * 7 * 64])
    h_fc1 = tf.nn.relu(tf.matmul(h_pool2_flat, W_fc1) + b_fc1)
    h_fc1_drop = tf.nn.dropout(h_fc1, keep_prob) 
    # 4
    W_fc2 = weigth_variable([1024, 10])
    b_fc2 = bias_varibale([10])#10个0.1组成的一行
    prediction = tf.nn.softmax(tf.matmul(h_fc1_drop, W_fc2) + b_fc2,name="output")#这是输出吗
    return prediction


def convert2gray(img):
    if len(img.shape) > 2:
        gray = np.mean(img, -1)
        return gray
    else:
        return img

def text2vec(text):
    text_len = len(text)
    vector = np.zeros(1 * CHAR_SET_LEN)

    def char2pos(c):
        if c == '_':
            k = 62
            return k
        k = ord(c) - 48
        if k > 9:
            k = ord(c) - 55
            if k > 35:
                k = ord(c) - 61
                if k > 61:
                    raise ValueError('No Map')
        return k

    for i, c in enumerate(text):
        idx = i * CHAR_SET_LEN + char2pos(c)
        vector[idx] = 1
    return vector

def vec2text(vec):
    char_pos = vec.nonzero()[0]
    text = []
    for i, c in enumerate(char_pos):
        char_at_pos = i 
        char_idx = c % CHAR_SET_LEN
        if char_idx < 10:
            char_code = char_idx + ord('0')
        elif char_idx < 36:
            char_code = char_idx - 10 + ord('A')
        elif char_idx < 62:
            char_code = char_idx - 36 + ord('a')
        elif char_idx == 62:
            char_code = ord('_')
        else:
            raise ValueError('error')
        text.append(chr(char_code))
    return "".join(text)

def get_next_batch(batch_size, each, images, labels):
    batch_x = np.zeros([batch_size, IMAGE_HEIGHT * IMAGE_WIDTH])
    batch_y = np.zeros([batch_size, 10])

    def get_text_and_image(i, each):
        image_num = each * batch_size + i
        label = labels[image_num]
        image_path = images[image_num]
        captcha_image = Image.open(image_path)
        captcha_image = np.array(captcha_image)
        return label, captcha_image

    for i in range(batch_size):
        text, image = get_text_and_image(i, each)
        image = convert2gray(image)

        batch_x[i, :] = image.flatten() / 255
        batch_y[i, :] = text2vec(text)
    return batch_x, batch_y

def get_random_batch(batch_size, images, labels,IMAGE_MUMBER = IMAGE_MUMBER):
    batch_x = np.zeros([batch_size, IMAGE_HEIGHT * IMAGE_WIDTH])
    batch_y = np.zeros([batch_size, 1 * CHAR_SET_LEN])

    def get_captcha_text_and_image(i):
        image_num = i
        label = labels[image_num]
        image_path = images[image_num]
        captcha_image = Image.open(image_path)
        captcha_image = np.array(captcha_image)
        return label, captcha_image

    for i in range(batch_size):
        text, image = get_captcha_text_and_image(random.randint(0, IMAGE_MUMBER - 1))
        image = convert2gray(image)
        batch_x[i, :] = image.flatten() / 255 
        batch_y[i, :] = text2vec(text)
    return batch_x, batch_y

def compute_accuracy(v_xs, v_ys, sess): 
    global prediction
    y_pre = sess.run(prediction, feed_dict={xs: v_xs, keep_prob: 1})
    correct_prediction = tf.equal(tf.argmax(y_pre, 1), tf.argmax(v_ys, 1))
    accuracy = tf.reduce_mean(tf.cast(correct_prediction, tf.float32))
    result = sess.run(accuracy, feed_dict={xs: v_xs, ys: v_ys, keep_prob: 1})
    return result


prediction = code_cnn()


def train_code_cnn(image_paths,pb_file_path,labels):
    global prediction
 
    cross_entropy = tf.reduce_mean(-tf.reduce_sum(ys * tf.log(prediction), reduction_indices=[1]))
    train_step = tf.train.AdamOptimizer(1e-4).minimize(cross_entropy)
    sess = tf.Session()

    init = tf.global_variables_initializer()
    sess.run(init)
    for epoch in range(EPOCH):
        
        for each in range(int(IMAGE_MUMBER / BATCH_SIZE)):
            batch_x, batch_y = get_next_batch(BATCH_SIZE, each, image_paths, labels)
            _, loss_ = sess.run([train_step, cross_entropy]
                                , feed_dict={xs: batch_x, ys: batch_y, keep_prob: 0.5})
            print("epoch: %d  iter: %d/%d   loss: %f"
                  % (epoch + 1, BATCH_SIZE * each, IMAGE_MUMBER, loss_))
    
        test_iamge_path = "F:/mnist/data/image/test/"
        test_labels_path = "F:/mnist/data/code_test_text.txt"
        test_image_paths, test_labels = \
            get_image_path_labels(test_iamge_path, test_labels_path, 200)
        batch_x_test, batch_y_test = \
            get_random_batch(BATCH_SIZE, test_image_paths, test_labels,200)
        accuracy_test = compute_accuracy(batch_x_test, batch_y_test, sess)
        print("test epoch: %d  acc: %f" % (epoch + 1, accuracy_test))

        batch_x_test, batch_y_test = get_random_batch(BATCH_SIZE, image_paths, labels)
        accuracy = compute_accuracy(batch_x_test, batch_y_test, sess)
        print("train epoch: %d  acc: %f" % (epoch + 1, accuracy))
        # save as a pd file
        constant_graph = graph_util.convert_variables_to_constants(sess, sess.graph_def, ["output"])
        with tf.gfile.FastGFile(pb_file_path, mode='wb') as f:
            f.write(constant_graph.SerializeToString())


def getStrContent(path):
    return open(path, 'r').read()

def get_image_path_labels(IMAGE_PATH=IMAGE_PATH, LABEL_PATH=LABEL_PATH, IMAGE_MUMBER=IMAGE_MUMBER):
    image_path = IMAGE_PATH
    label_path = LABEL_PATH
    image_paths = []
    for each in range(IMAGE_MUMBER):
        image_paths.append(image_path + str(each) + ".png")
    string = getStrContent(label_path)
    #labels = string.split("#")
    labels = string.split(",")
    return image_paths, labels


def main():
    image_paths, labels = get_image_path_labels()
    pb_file_path = "examplenet.pb"
    train_code_cnn(image_paths, pb_file_path, labels)


if __name__ == '__main__':
    main()

