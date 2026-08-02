"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

import { getArticles, deleteArticle } from "@/services/api";
import { Article } from "@/types/article";

import "./articles.css";


export default function ArticlesPage()
{
    const router = useRouter();

    const [articles, setArticles] = 
        useState<Article[]>([]);

    const [search, setSearch] =
        useState("");


    useEffect(() =>
    {
        getArticles()
            .then(setArticles);

    }, []);


    async function handleDelete(id: string)
    {
        const confirmDelete =
            confirm("Удалить статью?");


        if (!confirmDelete)
            return;


        await deleteArticle(id);


        setArticles(prev =>
            prev.filter(article =>
                article.id !== id
            )
        );
    }


    const filteredArticles =
        articles.filter(article =>
            article.title
                .toLowerCase()
                .includes(
                    search.toLowerCase()
                )
        );


    return (
        <div className="page">

            <aside className="sidebar">

                <h2>
                    AI Assistant
                </h2>

                <nav>
                    <button className="active">
                        📚 Статьи
                    </button>

                    <button>
                        💬 Обращения
                    </button>

                    <button>
                        👥 Пользователи
                    </button>
                </nav>

            </aside>


            <main className="content-area">

                <div className="header">

                    <div>
                        <h1>
                            База знаний
                        </h1>

                        <p>
                            Управление знаниями AI помощника
                        </p>
                    </div>


                    <button
                        className="create-button"
                        onClick={() =>
                            router.push(
                                "/articles/create"
                            )
                        }
                    >
                        + Добавить статью
                    </button>

                </div>


                <div className="stats">

                    <div className="stat-card">

                        <span>
                            Всего статей
                        </span>

                        <strong>
                            {articles.length}
                        </strong>

                    </div>


                    <div className="stat-card">

                        <span>
                            AI источник
                        </span>

                        <strong>
                            RAG
                        </strong>

                    </div>


                    <div className="stat-card">

                        <span>
                            Статус
                        </span>

                        <strong className="online">
                            ONLINE
                        </strong>

                    </div>

                </div>


                <input
                    className="search"
                    placeholder="Поиск статей..."
                    value={search}
                    onChange={e =>
                        setSearch(
                            e.target.value
                        )
                    }
                />


                {
                    filteredArticles.length === 0 &&
                    (
                        <div className="empty">

                            <h2>
                                Статей нет
                            </h2>

                            <p>
                                Создайте первую статью базы знаний
                            </p>

                        </div>
                    )
                }


                <div className="articles-grid">

                    {
                        filteredArticles.map(article =>
                        (
                            <div
                                className="article-card"
                                key={article.id}
                            >

                                <div className="article-header">

                                    <h2>
                                        {article.title}
                                    </h2>


                                    <div className="actions">

                                        <button
                                            onClick={() =>
                                                router.push(
                                                    `/articles/edit/${article.id}`
                                                )
                                            }
                                        >
                                            ✏️
                                        </button>


                                        <button
                                            onClick={() =>
                                                handleDelete(article.id)
                                            }
                                        >
                                            🗑️
                                        </button>

                                    </div>

                                </div>


                                <div className="tags">

                                    {
                                        article.keywords
                                            .split(",")
                                            .map(keyword =>
                                            (
                                                <span key={keyword}>
                                                    {keyword.trim()}
                                                </span>
                                            ))
                                    }

                                </div>


                                <p className="article-text">
                                    {article.content}
                                </p>

                            </div>
                        ))
                    }

                </div>

            </main>

        </div>
    );
}