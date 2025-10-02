import { BlogPostList } from "../components/BlogPostList";

export function meta() {
  return [
    { title: "Blog - Home" },
    { name: "description", content: "A simple blog application" },
  ];
}

export default function Home() {
  return <BlogPostList />;
}
