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

        res.status(200).json({ id: user.id, username:user.username })
       
    } catch (err) {
      next(err)
    }
}

exports.getUserCharacters = async (req,res,next) => 
{
    try
    {
        let { id } = req.body
        id = Number(id)
        if(!id || isNaN(id))
        {
            const error = new Error("Missing or wrong type of data!")
            error.status = 400
            throw error
        }
        const characters = await userService.getUserCharacters(id)
        console.log(characters)
        res.status(200).json(characters)
    } catch (err) {
        next(err)
    }
}

exports.createNewCharacter = async (req,res,next) =>
{
    try 
    {
        let { id, character_id, name } = req.body
        id = Number(id)
        character_id = Number(character_id)
        if(!id || isNaN(id) ||!character_id || isNaN(character_id) || !name)
        {
            const error = new Error("Missing or wrong type of data!")
            error.status = 400
            throw error
        }
        const result = await userService.createNewCharacter(id,character_id,name)
        if(!result)
            return res.status(400).json({error:"Hiba a karakter létrehozás közben!"})
        res.status(201).json({message: "Sikeres karakter létrehozás"})
    }catch(err)
    {
        next(err)
    }
}