# ShoppingApp

Usernames and passwords for tests:

jamesreid - applebeans12 //client

eliza29 - bluemoon! //client

jsantos - mypass123 //client

mtaylor - goudacheese10 //admin


---------------------------
Required features:

We have implement a log in and log out authentication. Usernames cannot be duplicated, therefore each user must have a unique username. Special characters or spaces are not
allowed in usernames but numbers are valid. If users attempt to add spaces or special characters in their username, the sign up button will be disabled until they type a valid username. On the other hand, passwords can contain special characters. We have created sample users in our SSMS in which one user is an admin.

Our customer features include viewing all products (including product name, quantity, price, and description). Users can modify the quantiy of a product they wish to buy, add and remove products from their carts, and buy a product right away or put the product in their cart. Users may click on their cart button at the top right to see the list of their purchases as well as their total.
Admin users could also view all products, however they may also modify the inventory by adding and removing products via a form table.


All of our data (users and products) are stored persistenly in a database. Products are updated regularly as clients buy products and admins upgrade the inventory by adding or removing procuts. With regards to passwords, the salt and hash of the passwords are stored in the database and accessed only when a user logs in.


Optional features:

We decided to implement a sign up feature in which users can create an account. This feature is strictly for clients, so accounts created through the sign up window will have
client features. This means admin users would be created internally as there is no option to sign up as an admin. However, if a user is indeed an admin, they will have additional features and accessibility, such as changing prices, adding/removing products, and updating product quantities.


Future considerations:
An interesting feature that could be considered for the future is implemeting a button that reveals the password that users have typed. This way, users could see if they have correctly entered their password or not. We would've also liked to implement a drop down menu where users could see their balance or perhaps add to their balance (wallet).
