import requests
import time
import socket
from _thread import *
coindata = {"BTC":0,"XRP":0,"XLM":0,"EOS":0}
endpoint = "https://api.binance.com"
APIKEY = ""
SECRETKEY = ""
HOST = ''
PORT = 9988
global streamLen
global check
check = False
streamLen=1024
def threaded_server(client_socket, addr): 
    print('Connected by :', addr[0], ':', addr[1]) 
    global streamLen
    # 클라이언트가 접속을 끊을 때 까지 반복합니다. 
    while True: 
        try:
            # 데이터가 수신되면 클라이언트에 다시 전송합니다.(에코)
            data = client_socket.recv(streamLen)
            if not data: 
                print('Disconnected by ' + addr[0],':',addr[1])
                break
            if str(data.decode()).isdigit():
                streamLen = int(data.decode())
                continue
            print("조은이에게: ",data.decode())
            #print('Received from ' + addr[0],':',addr[1] , data.decode())
            client_socket.send(data)

        except ConnectionResetError as e:

            print('Disconnected by ' + addr[0],':',addr[1])
            break
             
    client_socket.close() 
def connect(url):
    try:
        response = requests.get(url)
        if response.status_code == 200:
            return response
        else:
            return -1
    except:
        return -1
def readyConnect():
    while True:
        print('wait')
        client_socket, addr = server_socket.accept()
        start_new_thread(threaded_server, (client_socket, addr)) 
def getBinanceAPI():
    while(True):
        #print("getting data...")
        rs = connect(endpoint+"/api/v3/ticker/price")
        if not rs == "-1":
            data=rs.json()
            for i in range(0,len(data)-1):
                for j in ['EOSUSDT','BTCUSDT','XRPUSDT','XLMUSDT']:
                    if data[i]['symbol']==j:
                        coindata[j[0:len(j)-4]] = data[i]['price']
                        #print("save data...!")
        else:
            print("Error!")
        time.sleep(0.5)
        
if __name__ == '__main__':
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server_socket.bind((HOST, PORT)) 
    server_socket.listen() 
    print('server start')
    start_new_thread(readyConnect,())
    while True:
        getBinanceAPI()
    server_socket.close() 
