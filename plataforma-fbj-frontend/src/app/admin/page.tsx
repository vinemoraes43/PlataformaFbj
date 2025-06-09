import { AdminSideNavigation } from '@/components/ui/adminSideNavigation'

export default function AdminLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <div className="flex min-h-screen">
      <AdminSideNavigation />
      <div className="ml-64 p-8 w-full">
        {children}
      </div>
    </div>
  )
}