const userService = require('../services/userService')
const bcrypt = require('bcryptjs')

exports.registerUser = async (req,res,next) => {
    try {
        const { username, password } = req.body
        if (!username || !password)
        {
            const error = new Error("Missing inputs!")
            error.status = 400
            throw error
        }
        const existingUser = await userService.getUserFromUsername(username)
        if(existingUser)
            return res.status(500).json({ error: 'A felhasználónév már használatban van!'})
        const hashedPassword = await bcrypt.hash(password, 10)
        const result = await userService.createUser(username,hashedPassword)
        if(!result)
            return res.status(500).json({ error: 'Hiba a regisztráció során!' })

        res.status(201).json({ message: 'Sikeres regisztráció!' })
    } catch (err) {
        next(err)
    }
}
    
exports.loginUser = async (req,res,next) => {
    try {
        const { username, password } = req.body
        if (!username || !password)
        {
            const error = new Error("Missing inputs!")
            error.status = 400
            throw error
        }
        const user = await userService.getUserFromUsername(username)
        if(!user)
            return res.status(401).json({ error: 'Nem létezik ilyen felhasználó!' })
        if(!(await bcrypt.compare(password,user.password)))
            return res.status(401).json({ error: 'Hiba a bejelentkezés során!'})

        res.status(200).json({ message: 'Sikeres bejelentkezés!' })
       
    } catch (err) {
      next(err)
    }
}

exports.getUserCharacters = async (req,res,next) => 
{
    try
    {
        let { id } = req.body
        if(!id || isNaN(Number(id)))
        {
            const error = new Error("Missing or wrong type of data!")
            error.status = 400
            throw error
        }
        const characters = await userService.getUserCharacters(id)
        res.status(200).json(characters)
    } catch (err) {
        next(err)
    }
}