const express = require('express')
const app = express()
app.use(express.json())
app.use(express.urlencoded({extended:true}))

//ROUTES
const userRoutes = require('./api/routes/userRoutes')

app.use('/api/v1',userRoutes)

module.exports = app 