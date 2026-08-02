import { Article } from "@/types/article";

const API_URL = "http://localhost:5010";


export async function getArticles(): Promise<Article[]> {

    const response = await fetch(
        `${API_URL}/articles`,
        {
            credentials:"include"
        }
    );


    if (!response.ok) {
        console.log(response.status);
        throw new Error("Failed to fetch articles");
    }


    return response.json();
}


export async function createArticle(
    title:string,
    keywords:string,
    content:string
)
{

    const response = await fetch(
        `${API_URL}/articles`,
        {
            method:"POST",

            credentials:"include",

            headers:{
                "Content-Type":"application/json"
            },

            body:JSON.stringify({
                title,
                keywords,
                content
            })
        }
    );


    if(!response.ok)
    {
        throw new Error(
            "Failed to create article"
        );
    }


    return response.json();

}


export async function deleteArticle(id:string)
{

    const response = await fetch(
        `${API_URL}/articles/${id}`,
        {
            method:"DELETE",

            credentials:"include"
        }
    );


    if(!response.ok)
    {
        throw new Error(
            "Failed to delete article"
        );
    }

}


export async function getArticle(id:string)
{
    const response = await fetch(
        `${API_URL}/articles/${id}`,
        {
            credentials:"include"
        }
    );


    if(!response.ok)
    {
        throw new Error(
            "Failed to get article"
        );
    }


    return response.json();
}


export async function updateArticle(
    id:string,
    title:string,
    keywords:string,
    content:string
)
{

    const response = await fetch(
        `${API_URL}/articles/${id}`,
        {
            method:"PUT",

            credentials:"include",

            headers:{
                "Content-Type":"application/json"
            },

            body:JSON.stringify({
                title,
                keywords,
                content
            })
        }
    );


    if(!response.ok)
    {
        throw new Error(
            "Failed to update article"
        );
    }

    return;

}