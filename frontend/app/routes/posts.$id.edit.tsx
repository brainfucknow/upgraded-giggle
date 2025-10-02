import { useParams } from 'react-router';
import { BlogPostForm } from "../components/BlogPostForm";
import { useGetBlogPost } from "../hooks/useBlogPosts";

export function meta() {
  return [
    { title: "Edit Blog Post" },
  ];
}

export default function PostEdit() {
  const { id } = useParams();
  const postId = id ? parseInt(id, 10) : 0;
  const { data: post, isLoading, error } = useGetBlogPost(postId);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-8">
        <div className="text-gray-600">Loading...</div>
      </div>
    );
  }

  if (error || !post) {
    return (
      <div className="max-w-4xl mx-auto p-6">
        <div className="text-red-600">Post not found</div>
      </div>
    );
  }

  return <BlogPostForm post={post} isEdit={true} />;
}