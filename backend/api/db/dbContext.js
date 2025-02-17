require('dotenv').config()
const { Sequelize, DataTypes } = require('sequelize')

const sequelize = new Sequelize
(
    process.env.DB_NAME,
    process.env.DB_USER,
    process.env.DB_PASSWORD,
    {
        host: process.env.DB_HOST,
        dialect: process.env.DB_DIALECT,
        logging:false
    }
)

try
{
    sequelize.authenticate()
    console.log("db connected")
}catch(err)
{
    console.error("Error connecting to database")
}

const db = {}

db.Sequelize = Sequelize
db.sequelize = sequelize

const {User, Character, User_Character} = require('../models')(db.sequelize, DataTypes)
db.user = User
db.character = Character
db.user_character = User_Character

db.sequelize.sync({alter: true})

module.exports = db