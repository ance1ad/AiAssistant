import styles from "./AdminCard.module.css";


export default function AdminCard({
    title,
    children
}:{
    title:string;
    children:React.ReactNode;
})
{

return (

<div className={styles.container}>

    <div className={styles.card}>


        <h1 className={styles.title}>
            {title}
        </h1>


        {children}


    </div>

</div>

);

}