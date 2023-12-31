# from msg_broker import Rds
# from database import Database
from json import loads,dumps
import requests
# from models import User
from pprint import pprint
from hasher import hash
from getpass import getpass
import sys
from redis import Redis
from rq import Queue
from pymongo import MongoClient

class Rds:
    redis_conn = Redis(host="localhost" , port=6379)
    queue = Queue("mod_queue" , connection=redis_conn)

class Database:
    mongo_db = MongoClient("mongodb://localhost:80")
    db = mongo_db["User"]
    coll = db["Data"]

log = False

def process(message):
    results = Database.coll.find({"name" : message["name"]})
    for result in results:
        balance = result["balance"]
        # print(result)
    
        balance = balance + message["value"]
        Database.coll.update_one({"name" : message["name"]} , {"$set" : {"balance" : balance}})
        return {"Update" : "Successful"}
    return {"Error" : "Not found"}

def transaction(username : str , password : str):

    choice = "y"

    if choice == "y":
        payload={
            "name" : username,
            "password" : password
        }
        payload = dumps(payload)
        requests.post("http://localhost:8000/transactions" , data=payload)
        count = 0
        log = True

        while log:
            message_json = Rds.redis_conn.brpop("mod_queue" , timeout=3)
            try:
                _,y = message_json
                message = loads(y)
                if message["name"] == username:
                    count = 0
                    process(message)
                else:
                    count = count + 1
                    Rds.redis_conn.lpush("mod_queue" , dumps(message))
                    if count == 50:
                        print("\nDONE\n")
                        break
            except:
                print("\nDone\n")
                break
    else:
        print("\nNe mai auzim")
        exit()

username = sys.argv[1]
password = sys.argv[2]

transaction(username , password)
# input()