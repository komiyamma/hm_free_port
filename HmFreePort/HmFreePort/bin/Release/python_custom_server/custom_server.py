import socket
import threading
import os
import mimetypes
import urllib.parse

class MyHTTPServer:
    def __init__(self, host='localhost', port=8000, root_dir='.'):
        self.host = host
        self.port = port
        self.root_dir = root_dir
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)  # アドレス再利用を許可
        self.server_socket.bind((self.host, self.port))
        self.server_socket.listen(5)
        print(f"Server listening on {self.host}:{self.port}, serving from {self.root_dir}")

    def run(self):
        while True:
            client_socket, client_address = self.server_socket.accept()
            client_thread = threading.Thread(target=self.handle_client, args=(client_socket,))
            client_thread.start()

    def handle_client(self, client_socket):
        try:
            request = client_socket.recv(1024).decode('utf-8')
            if not request:
                return

            method, path, protocol = self.parse_request(request)
            print(f"Request: {method} {path} {protocol}")  # デバッグ用

            filepath = os.path.join(self.root_dir, path.lstrip('/')) # ルートディレクトリからの相対パスを作成
            filepath = os.path.normpath(filepath)  # パスの正規化

            if os.path.isdir(filepath):
                filepath = os.path.join(filepath, 'index.html') # ディレクトリの場合、index.htmlを探す

            if os.path.exists(filepath) and not os.path.isdir(filepath):
                self.send_file(client_socket, filepath)
            else:
                self.send_error(client_socket, 404, "Not Found")

        except Exception as e:
            print(f"Error handling client: {e}")
            self.send_error(client_socket, 500, "Internal Server Error")
        finally:
            client_socket.close()


    def parse_request(self, request):
        lines = request.split('\r\n')
        first_line = lines[0].split(' ')
        method = first_line[0]
        path = urllib.parse.unquote(first_line[1]) # URLエンコードされたパスをデコード
        protocol = first_line[2]
        return method, path, protocol

    def send_file(self, client_socket, filepath):
        try:
            content_type, _ = mimetypes.guess_type(filepath)
            with open(filepath, 'rb') as f:
                content = f.read()

            # Content-Type に text が含まれていたら charset=utf-8 を追加
            if content_type and 'text' in content_type:
                content_type += '; charset=utf-8'
            
            response = f"HTTP/1.1 200 OK\r\nContent-Type: {content_type}\r\nContent-Length: {len(content)}\r\n\r\n".encode('utf-8') + content

            client_socket.sendall(response)

        except FileNotFoundError:
            self.send_error(client_socket, 404, "Not Found")
        except Exception as e:
            print(f"Error sending file: {e}")
            self.send_error(client_socket, 500, "Internal Server Error")

    def send_error(self, client_socket, status_code, message):
        response = f"HTTP/1.1 {status_code} {message}\r\nContent-Type: text/plain; charset=utf-8\r\n\r\n{status_code} {message}".encode('utf-8')
        client_socket.sendall(response)


if __name__ == '__main__':
    import argparse

    parser = argparse.ArgumentParser(description='Simple HTTP Server')
    parser.add_argument('--host', default='localhost', help='Host address (default: localhost)')
    parser.add_argument('--port', type=int, default=8000, help='Port number (default: 8000)')
    parser.add_argument('--root_dir', default='.', help='Root directory to serve (default: .)')
    args = parser.parse_args()

    server = MyHTTPServer(host=args.host, port=args.port, root_dir=args.root_dir)
    server.run()