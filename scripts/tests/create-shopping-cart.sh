#!/bin/bash
curl -X POST -H "Content-Type:application/json" -d @create-shopping-cart.json http://localhost:5030/api/carts/add-items/c141fcd0-f534-4743-999b-e36dd984a645
