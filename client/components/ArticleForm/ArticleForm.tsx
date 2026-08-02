"use client";

import {
    useState
} from "react";

import styles from "./ArticleForm.module.css";


type Props = {

    initialTitle?: string;

    initialKeywords?: string;

    initialContent?: string;


    onSubmit:
    (
        title: string,
        keywords: string,
        content: string
    ) => Promise<void>;


    buttonText: string;

};



export default function ArticleForm({

    initialTitle = "",

    initialKeywords = "",

    initialContent = "",


    onSubmit,

    buttonText

}: Props)
{


    const [title, setTitle] =
        useState(initialTitle);


    const [keywords, setKeywords] =
        useState(initialKeywords);


    const [content, setContent] =
        useState(initialContent);



    async function handleSubmit(
        e: React.FormEvent
    )
    {
        e.preventDefault();


        await onSubmit(
            title,
            keywords,
            content
        );
    }



    return (

        <form
            className={styles.form}
            onSubmit={handleSubmit}
        >


            <label className={styles.label}>
                Название статьи
            </label>


            <input

                className={styles.input}

                value={title}

                placeholder="Например: Не проходит платеж"

                onChange={
                    e =>
                    setTitle(
                        e.target.value
                    )
                }

            />



            <label className={styles.label}>
                Ключевые слова
            </label>


            <input

                className={styles.input}

                value={keywords}

                placeholder="платеж, карта, ошибка"

                onChange={
                    e =>
                    setKeywords(
                        e.target.value
                    )
                }

            />



            <label className={styles.label}>
                Ответ пользователю
            </label>


            <textarea

                className={styles.textarea}

                value={content}

                placeholder="Введите текст ответа..."

                onChange={
                    e =>
                    setContent(
                        e.target.value
                    )
                }

            />



            <button

                className={styles.button}

                type="submit"

            >

                {buttonText}

            </button>


        </form>

    );

}