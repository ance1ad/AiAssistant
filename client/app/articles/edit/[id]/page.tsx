"use client";

import {
    useEffect,
    useState
} from "react";


import {
    getArticle,
    updateArticle
} from "@/services/api";


import {
    useParams,
    useRouter
} from "next/navigation";

import { Article } from "@/types/article";

import ArticleForm 
from "@/components/ArticleForm/ArticleForm";


import AdminCard 
from "@/components/AdminCard/AdminCard";



export default function EditArticlePage()
{


    const params = useParams();

    const router = useRouter();


    const id = params.id as string;



    const [article, setArticle] =
    useState<Article | null>(null);



    useEffect(()=>{


        getArticle(id)

            .then(setArticle);


    },[id]);



    if(!article)
    {
        return (

            <div>
                Загрузка...
            </div>

        );
    }



    async function handleUpdate(
        title:string,
        keywords:string,
        content:string
    )
    {

        await updateArticle(

            id,

            title,

            keywords,

            content

        );


        router.push("/articles");

    }



    return (

        <AdminCard
            title="Редактирование статьи"
        >


            <ArticleForm


                initialTitle={
                    article.title
                }


                initialKeywords={
                    article.keywords
                }


                initialContent={
                    article.content
                }



                buttonText="Сохранить изменения"



                onSubmit={
                    handleUpdate
                }


            />


        </AdminCard>

    );

}