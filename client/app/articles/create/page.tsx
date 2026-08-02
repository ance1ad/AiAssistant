"use client";


import {
    createArticle
} from "@/services/api";


import {
    useRouter
} from "next/navigation";


import ArticleForm 
from "@/components/ArticleForm/ArticleForm";


import AdminCard 
from "@/components/AdminCard/AdminCard";



export default function CreateArticlePage()
{

    const router = useRouter();



    async function handleCreate(
        title:string,
        keywords:string,
        content:string
    )
    {

        await createArticle(
            title,
            keywords,
            content
        );


        router.push("/articles");

    }



    return (

        <AdminCard 
            title="Создание статьи"
        >

            <ArticleForm

                buttonText="Создать статью"

                onSubmit={handleCreate}

            />

        </AdminCard>

    );

}