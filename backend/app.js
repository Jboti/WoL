const express = require('express')
const app = express()
app.use(express.json())
app.use(express.urlencoded({extended:true}))

//ROUTES
const userRoutes = require('./api/routes/userRoutes')
const errorHandler = require('./api/middlewares/errorHandler')

app.use('/api/v1',userRoutes)


app.use(errorHandler.notFoundError)
app.use(errorHandler.showError)

module.exports = app 