
import socket
import time 
 
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)  
sock.connect(('localhost', 8888))  
 
time.sleep(2) 
#print 'send ...'
#sock.send('yang^1002^1002^0001^00'+"\r\n")  
print 'recv...'
file_object = open("file.txt",'w+')
while True:
     data = sock.recv(1024)
     print data
     file_object.write(data)
print 'close...'
time.sleep(2)  
sock.close() 
file_object.close()

