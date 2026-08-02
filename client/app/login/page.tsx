"use client";


import {
    useState
} from "react";


import {
    login
} from "@/services/auth";


import {
    useRouter
} from "next/navigation";


import "./login.css";



export default function LoginPage()
{
    const router = useRouter();


    const [username, setUsername] =
        useState("");


    const [password, setPassword] =
        useState("");



    const [error, setError] =
        useState("");



    async function handleLogin()
    {
        try
        {
            await login(
                username,
                password
            );


            router.push("/articles");

        }
        catch
        {
            setError(
                "Неверный логин или пароль"
            );
        }
    }



    return (

        <div className="login-page">


            <div className="login-card">


                <div className="login-header">

                    <h1>
                        AI Assistant
                    </h1>


                    <p>
                        Панель управления
                    </p>

                </div>




                {
                    error &&
                    (
                        <div className="error">
                            {error}
                        </div>
                    )
                }




                <label>
                    Логин
                </label>


                <input

                    placeholder="Введите логин"

                    value={username}

                    onChange={
                        e =>
                        setUsername(
                            e.target.value
                        )
                    }

                />




                <label>
                    Пароль
                </label>


                <input

                    type="password"

                    placeholder="Введите пароль"

                    value={password}

                    onChange={
                        e =>
                        setPassword(
                            e.target.value
                        )
                    }

                />





                <button
                    onClick={handleLogin}
                >
                    Войти
                </button>



            </div>


        </div>

    );
}