export async function login(
    username:string,
    password:string
)
{
    const response = await fetch(
        "http://localhost:5010/auth/login",
        {
            method:"POST",
            headers:{
                "Content-Type":"application/json"
            },
            credentials:"include",
            body:JSON.stringify({
                username,
                password
            })
        }
    );


    if(!response.ok)
        throw new Error("Login failed");
}