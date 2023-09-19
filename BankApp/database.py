from pymongo import MongoClient
import sys

class Database:
    mongo_db = MongoClient("mongodb://localhost:80")
    db = mongo_db["User"]
    coll = db["Data"]

if __name__ == "__main__":
    user = sys.argv[1]
    results = Database.coll.find({"name" : user})
    for result in results:
        print(result["balance"])